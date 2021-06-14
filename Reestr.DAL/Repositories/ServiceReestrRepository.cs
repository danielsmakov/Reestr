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
        readonly string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public ServiceReestr Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    ServiceReestr entity = _con.QuerySingle<ServiceReestr>("SELECT * FROM ServiceReestr WHERE Id = @Id",
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
        public List<ServiceReestr> List(IQuery queryModel)
        {
            var query = queryModel as ServiceReestrQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.OrganizationName)) where += " AND Organizations.Name like '%@OrganizationName%'";
                    if (!string.IsNullOrEmpty(query.ServiceName)) where += " AND Services.Name like '%@ServiceName%'";

                    List<ServiceReestr> orgs = _con.Query<ServiceReestr>($"SELECT ServiceReestr.Id, ServiceReestr.OrganizationId, " +
                        $"ServiceReestr.ServiceId, ServiceReestr.Price, ServiceReestr.BeginDate FROM ServiceReestr " +
                        $"INNER JOIN Organizations ON ServiceReestr.OrganizationId = Organizations.Id " +
                        $"INNER JOIN Services ON ServiceReestr.ServiceId = Services.Id {where} " +
                        $"OFFSET (@Offset) ROWS FETCH NEXT @Limit ROWS ONLY",
                        new
                        {
                            OrganizationName = query.OrganizationName,
                            ServiceName = query.ServiceName,
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
        public void Insert(ServiceReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("INSERT INTO ServiceReestr (Id, OrganizationId, ServiceId, Price, BeginDate) VALUES (@Id, @OrganizationId, @ServiceId, @Price, @BeginDate)", new { entity });
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
                    _con.Execute("UPDATE ServiceReestr SET OrganizationId = @OrganizationId, ServiceId = @ServiceId, Price = @Price, BeginDate = @BeginDate WHERE Id = @Id", new { entity });
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
                    _con.Execute("UPDATE ServiceReestr SET EndDate = GETDATE() WHERE Id = @Id", new { id });
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
