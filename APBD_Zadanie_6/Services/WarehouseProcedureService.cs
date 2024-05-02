﻿using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using APBD_Task_6.Models;
using Microsoft.Extensions.Configuration;

namespace APBD_Zadanie_6.Services
{
    public class WarehouseProcedureService : IWarehouseProcedureService
    {
        private readonly IConfiguration _configuration;

        public WarehouseProcedureService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddProductToWarehouseAsync(ProductWarehouse productWarehouse)
        {
            var connectionString = _configuration.GetConnectionString("Database");
            using var connection = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("AddProductToWarehouse", connection);

            cmd.Connection = connection;
            await connection.OpenAsync();

            var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            cmd.Transaction = transaction;

            try
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdProduct", productWarehouse.IdProduct);
                cmd.Parameters.AddWithValue("IdWarehouse", productWarehouse.IdWarehouse);
                cmd.Parameters.AddWithValue("Amount", productWarehouse.Amount);
                cmd.Parameters.AddWithValue("CreatedAt", productWarehouse.CreatedAt);

               // await connection.OpenAsync();
                int rowsChanged = await cmd.ExecuteNonQueryAsync();

                if (rowsChanged < 1) throw new Exception();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }

            cmd.Parameters.Clear();
            using var cmd2 = new SqlCommand();
            cmd2.Connection = connection;

            cmd2.CommandText = "SELECT TOP 1 IdProductWarehouse FROM Product_Warehouse ORDER BY IdProductWarehouse DESC";

            using var reader = await cmd2.ExecuteReaderAsync();
            await reader.ReadAsync();
            int idProductWarehouse = int.Parse(reader["IdProductWarehouse"].ToString());
            await reader.CloseAsync();

            return idProductWarehouse;
        }
    }
}
