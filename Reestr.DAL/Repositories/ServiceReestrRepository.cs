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
    class ServiceReestrRepository : IRepository<ServiceReestr>
    {
        string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public ServiceReestr Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    ServiceReestr entity = _con.QuerySingle<ServiceReestr>("Select * From ServiceReestr Where Id = @Id",
                        new
                        {
                            id = id
                        }
                        );
                    _con.Close();
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<ServiceReestr> List(OrganizationQuery query)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.Name)) where += " AND Name like '%@Name%'";

                    List<ServiceReestr> orgs = _con.Query<ServiceReestr>($"Select * From ServiceReestr {where}",
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
        public void Insert(ServiceReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Insert Into ServiceReestr (Id, OrganizationId, ServiceId, Price, BeginDate, EndDate) Values (@Id, @OrganizationId, @ServiceId, @Price, @BeginDate, @EndDate)", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(ServiceReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Update ServiceReestr Set OrganizationId = @OrganizationId, ServiceId = @ServiceId, Price = @Price, BeginDate = @BeginDate Where Id = @Id", new { entity });
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
                    _con.Execute("Update ServiceReestr Set EndDate = GETDATE() Where Id = @Id", new { id });
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
