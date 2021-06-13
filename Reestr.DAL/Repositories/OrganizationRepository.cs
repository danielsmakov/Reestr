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
    public class OrganizationRepository : IRepository<Organization>
    {
        string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public Organization Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    Organization org = _con.QuerySingle<Organization>("Select * From Organizations Where Id = @Id",
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
        public List<Organization> List(OrganizationQuery query)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += where + " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.Name)) where += where + " AND Name like '%@Name%'";

                    List<Organization> orgs = _con.Query<Organization>($"Select * From Organizations {where}",
                        new
                        {
                            name = query.Name
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

    }
}
