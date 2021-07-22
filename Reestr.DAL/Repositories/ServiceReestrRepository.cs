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
                catch (Exception)
                {
                    throw new ApplicationException();
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

                    var where = ConfigureWhereClause(query);
                    var orderBy = " ORDER BY sr.BeginDate DESC";
                    

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

                    string sqlQuery = $@"SELECT sr.Id, sr.Price, sr.BeginDate, 
                        sr.OrganizationId, o.Id, o.Name, o.BIN, o.PhoneNumber, o.BeginDate, 
                        sr.ServiceId, s.Id, s.Name, s.Code, s.Price, s.BeginDate FROM ServiceReestr sr 
                        INNER JOIN Organizations o ON o.Id = sr.OrganizationId 
                        INNER JOIN Services s ON sr.ServiceId = s.Id {where} 
                        {orderBy} 
                        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

                    List<ServiceReestr> orgs = _con.Query<ServiceReestr, Organization, Service, ServiceReestr>(sqlQuery, 
                    (sr, o, s) =>
                    {
                        sr.Organization = o;
                        sr.Service = s;
                        return sr;
                    }, query, splitOn: "OrganizationId, ServiceId").ToList();

                    _con.Close();
                    return orgs;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException();
                }
            }
        }

        public int CountRecords(IQuery queryModel)
        {
            var query = queryModel as ServiceReestrQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = ConfigureWhereClause(query);

                    int totalRecords = _con.QuerySingle<int>($@"SELECT COUNT(*) FROM ServiceReestr sr 
                    INNER JOIN Organizations o ON o.Id = sr.OrganizationId 
                    INNER JOIN Services s ON sr.ServiceId = s.Id 
                    {where}", query);

                    _con.Close();
                    return totalRecords;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException();
                }
            }
        }

        public void Insert(ServiceReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = @"INSERT INTO ServiceReestr (OrganizationId, ServiceId, Price, BeginDate) VALUES (@OrganizationId, @ServiceId, @Price, @BeginDate); 
                    SELECT CAST(SCOPE_IDENTITY() AS int)";

                SqlTransaction transaction = null;

                _con.Open();
                transaction = _con.BeginTransaction();

                try
                {
                    int id = _con.Query<int>(sqlQuery, entity, transaction: transaction).First();
                    entity.Id = id;

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new ApplicationException();
                }
                finally
                {
                    _con.Close();
                }
            }
        }
        public void Update(ServiceReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = "UPDATE ServiceReestr SET OrganizationId = @OrganizationId, ServiceId = @ServiceId, Price = @Price, BeginDate = @BeginDate WHERE Id = @Id";

                SqlTransaction transaction = null;

                _con.Open();
                transaction = _con.BeginTransaction();

                try
                {
                    _con.Execute(sqlQuery, entity, transaction: transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new ApplicationException();
                }
                finally
                {
                    _con.Close();
                }
            }
        }
        public void Delete(int id)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = "UPDATE ServiceReestr SET EndDate = GETDATE() WHERE Id = @Id";

                SqlTransaction transaction = null;

                _con.Open();
                transaction = _con.BeginTransaction();

                try
                {
                    _con.Execute(sqlQuery, new { id }, transaction: transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new ApplicationException();
                }
                finally
                {
                    _con.Close();
                }
            }
        }

        private string ConfigureWhereClause(ServiceReestrQuery query)
        {

            try
            {
                var where = "WHERE 1=1";
                if (query.IsDeleted)
                {
                    where += " AND sr.EndDate is not null ";
                }
                else
                {
                    where += " AND sr.EndDate is null ";
                }
                if (!string.IsNullOrEmpty(query.OrganizationNameToSearchFor)) where += " AND o.Name like '%' + @OrganizationNameToSearchFor + '%'";
                if (!string.IsNullOrEmpty(query.ServiceNameToSearchFor)) where += " AND s.Name like '%' + @ServiceNameToSearchFor + '%'";

                return where;
            }
            catch
            {
                throw new ApplicationException();
            }
        }
    }
}
