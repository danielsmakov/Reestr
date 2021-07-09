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
    public class ServiceReestrRepository : IRepository<ServiceReestr>
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
                    var orderBy = " ORDER BY BeginDate DESC";
                    if (query.IsDeleted)
                    {
                        where += " AND EndDate is not null ";
                    }
                    else
                    {
                        where += " AND EndDate is null ";
                    }
                    if (!string.IsNullOrEmpty(query.OrganizationName)) where += " AND Organizations.Name like @OrganizationName";
                    if (!string.IsNullOrEmpty(query.ServiceName)) where += " AND Services.Name like @ServiceName";

                    if (!(query.SortingParameters is null))
                    {
                        if (!string.IsNullOrEmpty(query.SortingParameters[0]["field"]))
                        {
                            if (!string.IsNullOrEmpty(query.SortingParameters[0]["dir"]))
                            {
                                orderBy = $" ORDER BY {query.SortingParameters[0]["field"]} {query.SortingParameters[0]["dir"]}";
                            }
                        }
                    }

                    List<ServiceReestr> orgs = _con.Query<ServiceReestr>($"SELECT sr.Id, sr.OrganizationId, sr.ServiceId, sr.Price, sr.BeginDate, " +
                        $"o.Id, o.Name, o.BIN, o.PhoneNumber, o.BeginDate, " +
                        $"s.Id, s.Name, s.Code, s.Price, s.BeginDate FROM ServiceReestr sr" +
                        $"INNER JOIN Organizations o ON sr.OrganizationId = o.Id " +
                        $"INNER JOIN Services s ON sr.ServiceId = s.Id {where} " +
                        $"{orderBy} " +
                        $"OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY",

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
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganizationId", entity.OrganizationId);
                    param.Add("@ServiceId", entity.ServiceId);
                    param.Add("@Price", entity.Price);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("INSERT INTO ServiceReestr (OrganizationId, ServiceId, Price, BeginDate) VALUES (@OrganizationId, @ServiceId, @Price, @BeginDate)", param);
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
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", entity.Id);
                    param.Add("@OrganizationId", entity.OrganizationId);
                    param.Add("@ServiceId", entity.ServiceId);
                    param.Add("@Price", entity.Price);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("UPDATE ServiceReestr SET OrganizationId = @OrganizationId, ServiceId = @ServiceId, Price = @Price, BeginDate = @BeginDate WHERE Id = @Id", param);
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
