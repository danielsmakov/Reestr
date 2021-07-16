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
    public class EmployeeReestrRepository : IRepository<EmployeeReestr>
    {
        readonly string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public EmployeeReestr Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    EmployeeReestr entity = _con.QuerySingle<EmployeeReestr>("SELECT * FROM EmployeeReestr WHERE Id = @Id",
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
        public List<EmployeeReestr> List(IQuery queryModel)
        {
            var query = queryModel as EmployeeReestrQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = ConfigureWhereClause(query);
                    var orderBy = " ORDER BY er.BeginDate DESC";

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

                    string sqlQuery = $@"SELECT er.Id, er.IIN, er.FullName, er.DateOfBirth, er.PhoneNumber, er.BeginDate
                        er.OrganizationId, o.Id, o.Name, o.BIN, o.PhoneNumber, o.BeginDate FROM EmployeeReestr er
                        INNER JOIN Organizations o ON o.Id = er.OrganizationId
                        {where} {orderBy}
                        OFFSER @Offser ROWS FETCH NEXT @Limit ROWS ONLY";

                    List<EmployeeReestr> orgs = _con.Query<EmployeeReestr, Organization, EmployeeReestr>(sqlQuery,
                    (er, o) =>
                    {
                        er.Organization = o;
                        return er;
                    }, query, splitOn: "OrganizationId").ToList();

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
            var query = queryModel as EmployeeReestrQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = ConfigureWhereClause(query);

                    int totalRecords = _con.QuerySingle<int>($"SELECT COUNT(*) FROM ServiceReestr sr {where}", query);

                    _con.Close();
                    return totalRecords;
                }
                catch (Exception)
                {
                    throw new ApplicationException();
                }
            }
        }

        public void Insert(EmployeeReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = @"INSERT INTO EmployeeReestr (OrganizationId, IIN, FullName, DateOfBirth, PhoneNumber, BeginDate) VALUES (@OrganizationId, @IIN, @FullName, @DateOfBirth @PhoneNumber, @BeginDate); 
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
        public void Update(EmployeeReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                string sqlQuery = "UPDATE EmployeeReestr SET OrganizationId = @OrganizationId, IIN = @IIN,, FullName = @FullName, DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber, BeginDate = @BeginDate WHERE Id = @Id";

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
                string sqlQuery = "UPDATE EmployeeReestr SET EndDate = GETDATE() WHERE Id = @Id";

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

        public string ConfigureWhereClause(EmployeeReestrQuery query)
        {
            try
            {
                var where = "WHERE 1=1";
                if (query.IsDeleted)
                {
                    where += " AND er.EndDate is not null ";
                }
                else
                {
                    where += " AND er.EndDate is null ";
                }
                if (!string.IsNullOrEmpty(query.FullName)) where += " AND er.FullName like @FullName";
                if (!string.IsNullOrEmpty(query.OrganizationNameToSearchFor)) where += " AND o.Name like '%' + @OrganizationNameToSearchFor + '%'";
                if (!string.IsNullOrEmpty(query.FullNameToSearchFor)) where += " AND er.FullName like '%' + @FullNameToSearchFor + '%'";
                if (!string.IsNullOrEmpty(query.IIN)) where += " AND er.IIN LIKE @BIN";
                if (query.Id != 0) where += " AND er.Id NOT like @Id";

                return where;
            }
            catch
            {
                throw new ApplicationException();
            }
        }
    }
}
