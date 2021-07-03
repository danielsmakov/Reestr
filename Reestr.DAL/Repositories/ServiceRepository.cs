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
    public class ServiceRepository : IRepository<Service>
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

                    var where = ConfigureWhereClause(query);
                    var orderBy = " ORDER BY BeginDate DESC";
                    
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

                    List<Service> orgs = _con.Query<Service>($"SELECT * FROM Services {where} " +
                        $"{orderBy} " +
                        $"OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY", query).ToList();
                    _con.Close();
                    return orgs;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int CountRecords(IQuery queryModel)
        {
            var query = queryModel as ServiceQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = ConfigureWhereClause(query);

                    int totalRecords = _con.QuerySingle<int>($"SELECT COUNT(*) FROM Services {where}", query);

                    _con.Close();
                    return totalRecords;
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
                    _con.Execute("INSERT INTO Services (Name, Code, Price, BeginDate) VALUES (@Name, @Code, @Price, @BeginDate)", entity);
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
                    _con.Execute("UPDATE Services SET Name = @Name, Code = @Code, Price = @Price, BeginDate = @BeginDate WHERE Id = @Id", entity);
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
                    _con.Execute("UPDATE Services SET EndDate = GETDATE() WHERE Id = @Id", new { id });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private string ConfigureWhereClause(ServiceQuery query)
        {
            var where = "WHERE 1=1";

            if (query.IsDeleted)
            {
                where += " AND EndDate is NOT NULL ";
            }
            else
            {
                where += " AND EndDate is NULL ";
            }
            if (!string.IsNullOrEmpty(query.Name)) where += " AND Name LIKE @Name";
            if (!string.IsNullOrEmpty(query.NameToSearchFor)) where += $" AND Name LIKE '%{query.NameToSearchFor}%'";
            if (!string.IsNullOrEmpty(query.Code)) where += " AND Code LIKE @Code";
            if (query.Id != 0) where += " AND Id NOT like @Id";

            return where;
        }
    }
}
