using Dapper;
using Reestr.DAL.Entities;
using Reestr.DAL.Interfaces;
using Reestr.DAL.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Repositories
{
    public class OrganizationRepository : IRepository<Organization>
    {
        readonly string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public Organization Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    Organization org = _con.QuerySingle<Organization>("SELECT * FROM Organizations WHERE Id = @Id",
                        new
                        {
                            id = id
                        }
                        );
                    _con.Close();
                    return org;
                }
                catch (Exception)
                {
                    throw new ApplicationException();
                }
            }
        }
        public List<Organization> List(IQuery queryModel)
        {
            var query = queryModel as OrganizationQuery;

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


                    List<Organization> orgs = _con.Query<Organization>($"SELECT * FROM Organizations {where} " +
                        $"{orderBy} " +
                        $"OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY", query).ToList();
                    _con.Close();
                    return orgs;
                }
                catch (Exception)
                {
                    throw new ApplicationException();
                }
            }
        }

        public int CountRecords(IQuery queryModel)
        {
            var query = queryModel as OrganizationQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = ConfigureWhereClause(query);

                    int totalRecords = _con.QuerySingle<int>($"SELECT COUNT(*) FROM Organizations {where}", query);

                    _con.Close();
                    return totalRecords;
                }
                catch (Exception)
                {
                    throw new ApplicationException();
                }
            }
        }


        public void Insert(Organization entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = "INSERT INTO Organizations (Name, BIN, PhoneNumber, BeginDate) VALUES ( @Name, @BIN, @PhoneNumber, @BeginDate);" +
                    "SELECT CAST(SCOPE_IDENTITY() AS int)";

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
        public void Update(Organization entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = "UPDATE Organizations SET Name = @Name, BIN = @BIN, PhoneNumber = @PhoneNumber, BeginDate = @BeginDate WHERE Id = @Id";

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
                string sqlQuery = "UPDATE Organizations SET EndDate = GETDATE() WHERE Id = @Id";

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

        private string ConfigureWhereClause(OrganizationQuery query)
        {

            try
            {
                var where = "WHERE 1=1";
                if (query.IsDeleted)
                {
                    where += " AND EndDate is not null ";
                }
                else
                {
                    where += " AND EndDate is null ";
                }
                if (!string.IsNullOrEmpty(query.Name)) where += " AND Name LIKE @Name";
                if (!string.IsNullOrEmpty(query.NameToSearchFor)) where += $" AND Name LIKE '%' + @NameToSearchFor + '%'";
                if (!string.IsNullOrEmpty(query.BIN)) where += " AND BIN LIKE @BIN";
                if (query.Id != 0) where += " AND Id NOT like @Id";

                return where;
            }
            catch
            {
                throw new ApplicationException();
            }
        }
    }
}
