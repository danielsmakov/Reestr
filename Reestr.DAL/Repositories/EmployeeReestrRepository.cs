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
    class EmployeeReestrRepository : IRepository<EmployeeReestr>
    {
        string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
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
                catch (Exception ex)
                {
                    throw ex;
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

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.OrganizationName)) where += " AND Organizations.Name like '%@OrganizationName%'";
                    if (!string.IsNullOrEmpty(query.FullName)) where += " AND EmployeeReestr.FullName like '%@FullName%'";
                     
                    List<EmployeeReestr> orgs = _con.Query<EmployeeReestr>($"SELECT EmployeeReestr.Id, EmployeeReestr.OrganizationId, " +
                        $"EmployeeReestr.IIN, EmployeeReestr.FullName, EmployeeReestr.DateOfBirth, EmployeeReestr.PhoneNumber, " +
                        $"EmployeeReestr.BeginDate " +
                        $"FROM Organizations " +
                        $"INNER JOIN Organizations ON EmployeeReestr.OrganizationId = Organizations.Id {where} " +
                        $"OFFSET (@Offset) ROWS FETCH NEXT @Limit ROWS ONLY",
                        new
                        {
                            OrganizationName = query.OrganizationName,
                            FullName = query.FullName,
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
        public void Insert(EmployeeReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("INSERT INTO EmployeeReestr (Id, OrganizationId, IIN, FullName, DateOfBirth, PhoneNumber, BeginDate) VALUES (@Id, @OrganizationId, @IIN, @FullName, @DateOfBirth @PhoneNumber, @BeginDate)", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(EmployeeReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("UPDATE EmployeeReestr SET OrganizationId = @OrganizationId, IIN = @IIN,, FullName = @FullName, DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber, BeginDate = @BeginDate WHERE Id = @Id", new { entity });
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
                    _con.Execute("UPDATE EmployeeReestr SETEndDate = GETDATE() WHERE Id = @Id", new { id });
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
