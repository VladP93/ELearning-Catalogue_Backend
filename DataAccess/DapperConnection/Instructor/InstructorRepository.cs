using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess.DapperConnection.Instructor
{
    public class InstructorRepository : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;

        public InstructorRepository(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<IEnumerable<InstructorModel>> GetList()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storedProcedure = "usp_get_instructors";

            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storedProcedure,
                                                       null,
                                                       commandType: CommandType.StoredProcedure);
            }
            catch(Exception e)
            {
                throw new Exception("Error querying instructors list", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return instructorList;

        }
        public async Task<int> Create(string firstName, string lastName, string degree)
        {
            var storedProcedure = "usp_create_instructor";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var response = await connection.ExecuteAsync(storedProcedure,
                    new
                    {
                        InstructorId = Guid.NewGuid(),
                        FirstName = firstName,
                        LastName = lastName,
                        Degree = degree
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return response;
            }
            catch (Exception e)
            {
                _factoryConnection.CloseConnection();
                throw new Exception("Cannot insert a new instructor", e);
            }
        }

        public async Task<int> Update(Guid instructorId, string firstName, string lastName, string degree)
        {
            var storedProcedure = "usp_update_instructor";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var response = await connection.ExecuteAsync(storedProcedure,
                    new
                    {
                        InstructorId = instructorId,
                        FirstName = firstName,
                        LastName = lastName,
                        Degree = degree
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return response;
            }
            catch (Exception e)
            {
                _factoryConnection.CloseConnection();
                throw new Exception("Cannot update instructor", e);
            }
        }

        public async Task<int> Delete(Guid id)
        {
            var storedProcedure = "usp_delete_instructor";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var response = await connection.ExecuteAsync
                (
                    storedProcedure,
                    new {InstructorId = id},
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return response;
            }
            catch (Exception e)
            {
                _factoryConnection.CloseConnection();
                throw new Exception("Cannot delete instructor", e);
            }
        }

        public async Task<InstructorModel> GetById(Guid instructorId)
        {
            InstructorModel instructor = null;
            var storedProcedure = "usp_get_instructor_by_id";

            try
            {
                var connection = _factoryConnection.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>
                (
                    storedProcedure,
                    new
                    {
                        InstructorId = instructorId
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception e)
            {
                throw new Exception("Cannot found instructor", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            
            return instructor;
        }
    }
}