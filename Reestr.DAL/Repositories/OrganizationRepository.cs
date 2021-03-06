using Dapper;
using Reestr.DAL.Entities;
using Reestr.DAL.Interfaces;
using Reestr.DAL.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                catch (Exception ex)
                {
                    throw ex;
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

                    var where = "WHERE 1=1";
                    var orderBy = " ORDER BY (SELECT NULL)";
                    if (query.IsDeleted)
                    {
                        where += " AND EndDate is not null ";
                    }
                    else
                    {
                        where += " AND EndDate is null ";
                    }
                    if (!string.IsNullOrEmpty(query.Name)) where += " AND Name like @Name";
                    if (!string.IsNullOrEmpty(query.BIN)) where += " AND BIN like @BIN";
                    if (query.Id != 0) where += " AND Id NOT like @Id";
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
                        $"OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY",
                        new
                        {
                            Id = query.Id,
                            Name = query.Name,
                            Offset = query.Offset,
                            Limit = query.Limit,
                            BIN = query.BIN
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

        public int CountRecords(IQuery queryModel)
        {
            var query = queryModel as OrganizationQuery;

            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";

                    if (query.IsDeleted)
                    {
                        where += " AND EndDate is NOT NULL ";
                    }
                    else
                    {
                        where += " AND EndDate is NULL ";
                    }

                    int totalRecords = _con.QuerySingle<int>($"SELECT COUNT(*) FROM Organizations {where}");

                    _con.Close();
                    return totalRecords;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Insert(Organization entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Name", entity.Name);
                    param.Add("@BIN", entity.BIN);
                    param.Add("@PhoneNumber", entity.PhoneNumber);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("INSERT INTO Organizations (Name, BIN, PhoneNumber, BeginDate) VALUES ( @Name, @BIN, @PhoneNumber, @BeginDate)", param);
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(Organization entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", entity.Id);
                    param.Add("@Name", entity.Name);
                    param.Add("@BIN", entity.BIN);
                    param.Add("@PhoneNumber", entity.PhoneNumber);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("UPDATE Organizations SET Name = @Name, BIN = @BIN, PhoneNumber = @PhoneNumber, BeginDate = @BeginDate WHERE Id = @Id", param);
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
                    _con.Execute("UPDATE Organizations SET EndDate = GETDATE() WHERE Id = @Id", new { id });
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
