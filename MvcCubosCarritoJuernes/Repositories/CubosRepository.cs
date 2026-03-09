using Microsoft.EntityFrameworkCore;
using MvcCubosCarritoJuernes.Data;
using MvcCubosCarritoJuernes.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Collections.Generic;

#region STORED PROCEDURES
//DELIMITER $$
//create procedure SP_UPDATE_CUBO
//	(in idCubo int,
//    in p_nombre varchar(50),
//    in p_modelo varchar(50),
//    in p_marca varchar(50),
//    in p_imagen varchar(50),
//    in p_precio int)
//begin
//	update CUBOS set nombre = p_nombre, modelo = p_modelo, marca = p_marca, imagen = p_imagen, precio = p_precio where id_cubo = idCubo;
//end$$

//DELIMITER ;


//DELIMITER $$
//create procedure SP_INSERT_CUBO
//	(in p_nombre varchar(50),
//    in p_modelo varchar(50),
//    in p_marca varchar(50),
//    in p_imagen varchar(50),
//    in p_precio int)
//begin
//	DECLARE idCubo INT;

//SELECT id_cubo INTO idCubo FROM Cubos WHERE nombre = p_nombre LIMIT 1;

//SELECT COALESCE(MAX(id_cubo), 0) +1 INTO idCubo FROM CUBOS;

//insert into CUBOS values(idCubo, p_nombre, p_modelo, p_marca, p_imagen, p_precio);
//end$$

//DELIMITER ;

#endregion

namespace MvcCubosCarritoJuernes.Repositories
{
    public class CubosRepository
    {
        private CubosContext context;

        public CubosRepository(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            var consulta = from datos in this.context.Cubos
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<Cubo> GetCuboAsync(int idCubo)
        {
            var consulta = from datos in this.context.Cubos
                           where datos.IdCubo == idCubo
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task UpdateCuboAsync(Cubo cubo)
        {
            string sql = "CALL SP_UPDATE_CUBO(@idCubo, @p_nombre, @p_modelo, @p_marca, @p_imagen, @p_precio)";
            MySqlParameter pamIdCubo = new MySqlParameter("@idCubo", cubo.IdCubo);
            MySqlParameter pamNom = new MySqlParameter("@p_nombre", cubo.Nombre);
            MySqlParameter pamMod = new MySqlParameter("@p_modelo", cubo.Modelo);
            MySqlParameter pamMar = new MySqlParameter("@p_marca", cubo.Marca);
            MySqlParameter pamImg = new MySqlParameter("@p_imagen", cubo.Imagen);
            MySqlParameter pamPrecio = new MySqlParameter("@p_precio", cubo.Precio);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamIdCubo, pamNom, pamMod, pamMar, pamImg, pamPrecio);
        }

        public async Task InsertCuboAsync(Cubo cubo)
        {
            if (this.context.Cubos.Any())
            {
                int maxId = this.context.Cubos.Max(x => x.IdCubo);
                cubo.IdCubo = maxId + 1;
            }
            else
            {
                cubo.IdCubo = 1; 
            }
            await this.context.Cubos.AddAsync(cubo);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Cubo>> GetCubosSessionAsync(List<int> idsCubos)
        {
            var consulta = from datos in this.context.Cubos
                           where idsCubos.Contains(datos.IdCubo)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task ComprarAsync(Compra compra)
        {
            int idCompra = await this.context.Compras.MaxAsync(c => c.IdCompra);
            compra.IdCompra = compra.IdCompra + idCompra;
            compra.Cantidad = 1;
            compra.FechaPedido = DateTime.Now;
            await this.context.Compras.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }

    }
}
