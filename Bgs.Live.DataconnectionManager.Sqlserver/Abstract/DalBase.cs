using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bgs.DataConnectionManager.SqlServer.Abstract
{
    public abstract class DalBase<TCommand, TConnection>
        where TCommand : class, IDbCommand
        where TConnection : class, IDbConnection
    {
        private const int MaxParamsValue = 2000;

        protected TCommand GetCommand(bool fromArchive = false)
        {
            var connection = GetConnection(fromArchive);
            var command = connection.CreateCommand();

            return (TCommand)command;
        }

        protected TCommand GetCommand(string commandText, bool fromArchive = false)
        {
            return GetCommand(CommandType.Text, commandText, fromArchive);
        }

        protected TCommand GetCommand(CommandType commandType, bool fromArchive = false)
        {
            var command = GetCommand(fromArchive);

            command.CommandType = commandType;

            return command;
        }

        protected TCommand GetCommand(CommandType commandType, string commandText, bool fromArchive = false)
        {
            var command = GetCommand(commandType, fromArchive);

            command.CommandText = commandText;

            return command;
        }

        protected Task<TCommand> GetCommandAsync(string commandText, bool fromArchive = false, CancellationToken cancellationToken = default)
        {
            return GetCommandAsync(commandText, CommandType.Text, fromArchive, cancellationToken);
        }

        protected async Task<TCommand> GetCommandAsync(string commandText, CommandType commandType, bool fromArchive = false, CancellationToken cancellationToken = default)
        {
            var connection = await GetConnectionAsync(fromArchive, cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;

            return (TCommand)command;
        }

        protected TCommand GetSpCommand(bool fromArchive = false)
        {
            return GetCommand(CommandType.StoredProcedure, fromArchive);
        }

        protected TCommand GetSpCommand(string commandText, bool fromArchive = false)
        {
            return GetCommand(CommandType.StoredProcedure, commandText, fromArchive);
        }

        protected Task<TCommand> GetSpCommandAsync(string commandText, bool fromArchive = false, CancellationToken cancellationToken = default)
        {
            return GetCommandAsync(commandText, CommandType.StoredProcedure, fromArchive, cancellationToken);
        }

        protected TCommand GetSpReadCommand(string commandText)
        {
            return GetCommand(CommandType.StoredProcedure, commandText, true);
        }

        protected Task<TCommand> GetSpReadCommandAsync(string commandText, CancellationToken cancellationToken = default)
        {
            return GetSpCommandAsync(commandText, true, cancellationToken);
        }

        protected async Task<TCommand> GetSqlCommandAsync(Query query, bool fromArchive = false, CancellationToken cancellationToken = default)
        {
            var compiler = new SqlServerCompiler();
            var sqlResult = compiler.Compile(query);
            var commandText = sqlResult.Sql;
            var cmd = await GetCommandAsync(commandText, CommandType.Text, fromArchive, cancellationToken);

            for (var index = 0; index < sqlResult.Bindings.Count; index++)
            {
                var item = sqlResult.Bindings[index];

                var parameter = cmd.CreateParameter();
                parameter.ParameterName = $"p{index}";
                parameter.Value = item ?? DBNull.Value;

                cmd.Parameters.Add(parameter);
            }

            return cmd;
        }

        protected Query GetSqlKataQuery(string tableName)
        {
            return new QueryFactory().Query(tableName);
        }

        protected Query GetSqlKataQuery()
        {
            return new QueryFactory().Query();
        }

        public IEnumerable<K> ChunkCommand<T, K>(IEnumerable<T> chunkingBy, Func<IEnumerable<T>, IEnumerable<K>> command)
        {
            return ChunkCommand(MaxParamsValue, chunkingBy, command);
        }

        public IEnumerable<K> ChunkCommand<T, K>(int chunkSize, IEnumerable<T> chunkingBy, Func<IEnumerable<T>, IEnumerable<K>> command)
        {
            var totalData = new List<K>();

            var chunkedList = chunkingBy.ToList();

            for (var i = 0; i < chunkedList.Count; i += chunkSize)
            {
                var chunkedData = chunkedList.GetRange(i, Math.Min(chunkSize, chunkedList.Count - i));

                totalData.AddRange(command(chunkedData));
            }

            return totalData;
        }

        public Task<IEnumerable<K>> ChunkCommandAsync<T, K>(IEnumerable<T> chunkingBy, Func<IEnumerable<T>, Task<IEnumerable<K>>> command)
        {
            return ChunkCommandAsync(MaxParamsValue, chunkingBy, command);
        }

        public async Task<IEnumerable<K>> ChunkCommandAsync<T, K>(int chunkSize, IEnumerable<T> chunkingBy, Func<IEnumerable<T>, Task<IEnumerable<K>>> command)
        {
            var totalData = new List<K>();

            var chunkedList = chunkingBy.ToList();

            for (var i = 0; i < chunkedList.Count; i += chunkSize)
            {
                var chunkedData = chunkedList.GetRange(i, Math.Min(chunkSize, chunkedList.Count - i));

                totalData.AddRange(await command(chunkedData));
            }

            return totalData;
        }

        public Task ChunkCommandActionAsync<T>(IEnumerable<T> chunkingBy, Func<IEnumerable<T>, Task> command)
        {
            return ChunkCommandActionAsync(MaxParamsValue, chunkingBy, command);
        }

        public async Task ChunkCommandActionAsync<T>(int chunkSize, IEnumerable<T> chunkingBy, Func<IEnumerable<T>, Task> command)
        {
            var chunkedList = chunkingBy.ToList();

            for (var i = 0; i < chunkedList.Count; i += chunkSize)
            {
                var chunkedData = chunkedList.GetRange(i, Math.Min(chunkSize, chunkedList.Count - i));

                await command(chunkedData);
            }
        }

        public IDictionary<M, N> ChunkCommandDict<T, M, N>(IEnumerable<T> chunkingBy, Func<IEnumerable<T>, Dictionary<M, N>> command)
        {
            var totalData = new Dictionary<M, N>();

            var chunkedList = chunkingBy.ToList();

            for (int i = 0; i < chunkedList.Count; i += MaxParamsValue)
            {
                var chunkedData = chunkedList.GetRange(i, Math.Min(MaxParamsValue, chunkedList.Count - i));

                foreach (var keyValue in command(chunkedData))
                {
                    totalData.Add(keyValue.Key, keyValue.Value);
                }
            }

            return totalData;
        }

        public void ChunkCommandAction<T>(int chunkSize, IEnumerable<T> chunkingBy, Action<IEnumerable<T>> command)
        {
            ChunkCommand(chunkSize, chunkingBy, (chunck) =>
            {
                command(chunck);
                return new bool[0];
            });
        }

        protected abstract TConnection GetConnection(bool fromArchive);

        protected abstract Task<TConnection> GetConnectionAsync(bool fromArchive, CancellationToken cancellationToken = default);
    }
}