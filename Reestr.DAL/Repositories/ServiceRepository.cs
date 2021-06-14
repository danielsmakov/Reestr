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
                    Service service = _con.QuerySingle<Service>("Select * From Services Where Id = @Id",
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
        public List<Service> List(OrganizationQuery query)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.Name)) where += " AND Name like '%@Name%'";

                    List<Service> orgs = _con.Query<Service>($"Select * From Services {where}",
                        new
                        {
                            name = query.Name
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
        public void Insert(Service entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Insert Into Services (Id, Name, Code, Price, BeginDate, EndDate) Values (@Id, @Name, @Code, @Price, @BeginDate, @EndDate)", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(Service entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Update Services Set Name = @Name, Code = @Code, Price = @Price, BeginDate = @BeginDate Where Id = @Id", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Delete(int id)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Update Services Set EndDate = GETDATE() Where Id = @Id", new { id });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
