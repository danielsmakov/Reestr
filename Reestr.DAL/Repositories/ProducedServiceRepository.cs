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
    class ProducedServiceRepository : IRepository<ProducedService>
    {
        string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public ProducedService Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    ProducedService entity = _con.QuerySingle<ProducedService>("Select * From ProducedServices Where Id = @Id",
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
        public List<ProducedService> List(OrganizationQuery query)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.Name)) where += " AND Name like '%@Name%'";

                    List<ProducedService> orgs = _con.Query<ProducedService>($"Select * From ProducedServices {where}",
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
        public void Insert(ProducedService entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Insert Into ProducedServices (Id, OrganizationId, ServiceId, EmployeeId, BeginDate, EndDate) Values (@Id, @OrganizationId, @ServiceId, @EmployeeId, @BeginDate, @EndDate)", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(ProducedService entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Update ProducedServices Set OrganizationId = @OrganizationId, ServiceId = @ServiceId, EmployeeId = @EmployeeId, BeginDate = @BeginDate Where Id = @Id", new { entity });
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
                    _con.Execute("Update ProducedServices Set EndDate = GETDATE() Where Id = @Id", new { id });
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
