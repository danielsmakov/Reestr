using Dapper;
using Reestr.DAL.Entities;
using Reestr.DAL.Interfaces;
using Reestr.DAL.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Repositories
{
    class ServiceRepository : IRepository<Service>
    {
        string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public Service Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    Service service = _con.QuerySingle<Service>("SELECT * FROM Services WHERE Id = @Id",
                        new
                        {
                            id = id
                        }
                        );
                    _con.Close();
                    return service;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<Service> List(IQuery queryModel)
        {
            var query = queryModel as ServiceQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.Name)) where += " AND Name like '%@Name%'";

                    List<Service> orgs = _con.Query<Service>($"SELECT * FROM Services {where} " +
                        $"OFFSET (@Offset) ROWS FETCH NEXT @Limit ROWS ONLY",
                        new
                        {
                            Name = query.Name,
                            Offset = query.Offset,
                            Limit = query.Limit
                        }
                        ).ToList();
                    _con.Close();
                    return orgs;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public bool Insert(Service entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("INSERT IntINTO Services (Id, Name, Code, Price, BeginDate) VALUES (@Id, @Name, @Code, @Price, @BeginDate)", new { entity });
                    _con.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool Update(Service entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("UPDATE Services SET Name = @Name, Code = @Code, Price = @Price, BeginDate = @BeginDate WHERE Id = @Id", new { entity });
                    _con.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool Delete(int id)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("UPDATE Services SET EndDate = GETDATE() WHERE Id = @Id", new { id });
                    _con.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
