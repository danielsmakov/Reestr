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
                    if (query.IsDeleted)
                    {
                        where += " AND EndDate is not null ";
                    }
                    else
                    {
                        where += " AND EndDate is null ";
                    }
                    if (!string.IsNullOrEmpty(query.OrganizationName)) where += " AND Organizations.Name like @OrganizationName";
                    if (!string.IsNullOrEmpty(query.FullName)) where += " AND EmployeeReestr.FullName like @FullName";
                    if (!string.IsNullOrEmpty(query.IIN)) where += " AND IIN like @IIN";
                    if (query.Id != 0) where += " AND Id NOT like @Id";

                    List<EmployeeReestr> orgs = _con.Query<EmployeeReestr>($"SELECT EmployeeReestr.Id, EmployeeReestr.OrganizationId, " +
                        $"EmployeeReestr.IIN, EmployeeReestr.FullName, EmployeeReestr.DateOfBirth, EmployeeReestr.PhoneNumber, " +
                        $"EmployeeReestr.BeginDate " +
                        $"FROM Organizations " +
                        $"INNER JOIN Organizations ON EmployeeReestr.OrganizationId = Organizations.Id {where} " +
                        $"ORDER BY (SELECT NULL)" +
                        $"OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY",
                        new
                        {
                            Id = query.Id,
                            OrganizationName = query.OrganizationName,
                            FullName = query.FullName,
                            Offset = query.Offset,
                            Limit = query.Limit,
                            IIN = query.IIN
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
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganizationId", entity.OrganizationId);
                    param.Add("@IIN", entity.IIN);
                    param.Add("@FullName", entity.FullName);
                    param.Add("@DateOfBirth", entity.DateOfBirth);
                    param.Add("@PhoneNumber", entity.PhoneNumber);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("INSERT INTO EmployeeReestr (OrganizationId, IIN, FullName, DateOfBirth, PhoneNumber, BeginDate) VALUES (@OrganizationId, @IIN, @FullName, @DateOfBirth @PhoneNumber, @BeginDate)", param);
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
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", entity.Id);
                    param.Add("@OrganizationId", entity.OrganizationId);
                    param.Add("@IIN", entity.IIN);
                    param.Add("@FullName", entity.FullName);
                    param.Add("@DateOfBirth", entity.DateOfBirth);
                    param.Add("@PhoneNumber", entity.PhoneNumber);
                    param.Add("@BeginDate", entity.BeginDate);
                    _con.Open();
                    _con.Execute("UPDATE EmployeeReestr SET OrganizationId = @OrganizationId, IIN = @IIN,, FullName = @FullName, DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber, BeginDate = @BeginDate WHERE Id = @Id", param);
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
