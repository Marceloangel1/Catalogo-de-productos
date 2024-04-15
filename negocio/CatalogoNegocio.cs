using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using negocio;



namespace presentacion
{
      public class CatalogoNegocio
    {
        public List<Catalogo> listar()
        {

            List<Catalogo> lista = new List<Catalogo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;


            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl Imagen, A.Precio, M.Id, C.Id, A.Id from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = a.IdCategoria";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Catalogo aux = new Catalogo();
                    
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Id = (int)lector["id"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Precio = (decimal)lector["Precio"];                   
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)lector["Id"];
                    aux.Categoria.Descripcion = (string)lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)lector["Id"];
                    aux.Marca.Descripcion = (string)lector["Marca"];
                    if (!(lector["Imagen"] is DBNull))
                        aux.ImagenUrl  = (string)lector["Imagen"];

                  

                    lista.Add(aux);
                }

                conexion.Close();

                


                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

         
        }


        public void agregar(Catalogo nuevo)
        {
            accesoDatos datos  = new accesoDatos();
            
            try
            {
                datos.setearConsuta("insert into ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria, ImagenUrl)values('" + nuevo.Codigo + "','" + nuevo.Nombre + "','" + nuevo.Descripcion + "','" + nuevo.Precio + "', @idMarca, @idCategoria, @ImagenUrl)");

                datos.setearPrametro("@IdMarca", nuevo.Marca.Id);
                datos.setearPrametro("@IdCategoria", nuevo.Categoria.Id);
                datos.setearPrametro("@ImagenUrl", nuevo.ImagenUrl);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Catalogo cata)
        {
            accesoDatos datos = new accesoDatos();
            try
            {
                datos.setearConsuta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCate, ImagenUrl = @Imagen, Precio = @Precio where Id = @Id");

                datos.setearPrametro("@Codigo", cata.Codigo);
                datos.setearPrametro("@Nombre", cata.Nombre);
                datos.setearPrametro("Descripcion", cata.Descripcion);
                datos.setearPrametro("@IdMarca", cata.Marca.Id);
                datos.setearPrametro("@IdCate", cata.Categoria.Id);
                datos.setearPrametro("@Imagen", cata.ImagenUrl);
                datos.setearPrametro("@Precio", cata.Precio);
                datos.setearPrametro("@Id", cata.Id);

                datos.ejecutarAccion();
            }




            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void eliminar(int Id)
        {
            try
            {
                accesoDatos datos = new accesoDatos();
                datos.setearConsuta("delete from ARTICULOS where Id = @Id");
                datos.setearPrametro("@Id", Id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Catalogo> filtrar(string campo, string criterio, string filtro)
        {
            List<Catalogo> lista = new List<Catalogo>();
            accesoDatos datos = new accesoDatos();
            try
            {
                string consulta = ("select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl Imagen, A.Precio, M.Id, C.Id, A.Id from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = a.IdCategoria AND ");
               if (campo == "Precio")
                {

                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;

                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;

                        default:
                            consulta += "Precio = " + filtro;
                            break;


                    }




                }
               else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con ":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;

                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;


                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

               datos.setearConsuta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Catalogo aux = new Catalogo();

                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Id = (int)datos.Lector["id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["Id"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["Id"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    if (!(datos.Lector["Imagen"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["Imagen"];



                    lista.Add(aux);
                }




                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
       
   
}
