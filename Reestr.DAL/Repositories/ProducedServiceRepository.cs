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
    public class ProducedServiceRepository : IRepository<ProducedService>
    {
        readonly string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public ProducedService Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    ProducedService entity = _con.QuerySingle<ProducedService>("SELECT * FROM ProducedServices WHERE Id = @Id",
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
        public List<ProducedService> List(IQuery queryModel)
        {
            var query = queryModel as ProducedServiceQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";

                    List<ProducedService> orgs = _con.Query<ProducedService>($"SELECT * FROM ProducedServices {where} " +
                        $"ORDER BY (SELECT NULL)" +
                        $"OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY",
                        new
                        {
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
        public void Insert(ProducedService entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganizationId", entity.OrganizationId);
                    param.Add("@ServiceReestrId", entity.ServiceReestrId);
                    param.Add("@EmployeeId", entity.EmployeeId);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("INSERT INTO ProducedServices (OrganizationId, ServiceReestrId, EmployeeId, BeginDate) VALUES (@OrganizationId, @ServiceReestrId, @EmployeeId, @BeginDate)", param);
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
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", entity.Id);
                    param.Add("@OrganizationId", entity.OrganizationId);
                    param.Add("@ServiceReestrId", entity.ServiceReestrId);
                    param.Add("@EmployeeId", entity.EmployeeId);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("UPDATE ProducedServices SET OrganizationId = @OrganizationId, ServiceReestrId = @ServiceReestrId, EmployeeId = @EmployeeId, BeginDate = @BeginDate WHERE Id = @Id", param);
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
                    _con.Execute("UPDATE ProducedServices SET EndDate = GETDATE() WHERE Id = @Id", new { id });
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
