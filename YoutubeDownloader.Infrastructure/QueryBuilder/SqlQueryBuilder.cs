using EnsureThat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YoutubeDownloader.Domain.Common.Pagination;

namespace YoutubeDownloader.Infrastructure.QueryBuilder
{
    public class SqlQueryBuilder
    {
        private readonly IDbConnection _connection;

        private List<string> _columnsToSelect;
        private string _dataSource;
        private List<string> _whereConditions;
        private SqlQueryParameters _parameters;
        private SortColumn _sortingBy;
        private int? _topCount;
        private bool _isDistinct;
        private bool _isRandom;
        private bool _sum;


        public SqlQueryBuilder(IDbConnection connection)
        {
            _connection = connection;

            Reset();
        }

        public SqlQueryBuilder Select(params string[] columns)
        {
            EnsureArg.IsNotNull(columns, nameof(columns));

            var splitColumns = columns.Select(c => c.Trim());
            _columnsToSelect.AddRange(splitColumns);

            return this;
        }

        public SqlQueryBuilder SelectDistinct(params string[] columns)
        {
            _isDistinct = true;

            return Select(columns);
        }
        public SqlQueryBuilder OrderBy(string sortCriteria)
        {
            if (!string.IsNullOrWhiteSpace(sortCriteria))
            {
                _sortingBy = SortCriteria.Parse(sortCriteria);
            }

            return this;
        }
        public SqlQueryBuilder From(string dataSource)
        {
            Ensure.String.IsNotNullOrWhiteSpace(dataSource, nameof(dataSource));

            _dataSource = dataSource;

            return this;
        }

        public SqlQueryBuilder Where<T>(string column, T value) where T : struct
        {
            AddFilter(column, " = ", value);

            return this;
        }

        public SqlQueryBuilder Where<T>(string column, T? value) where T : struct
        {
            if (value == null)
            {
                return this;
            }

            AddFilter(column, " = ", value.Value);

            return this;
        }

        public SqlQueryBuilder Where(string column, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return this;
            }

            AddFilter(column, " = ", value);

            return this;
        }

        public SqlQueryBuilder WhereLike(string column, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return this;
            }
            AddFilter(column, " LIKE ", '%' + value + '%');

            return this;
        }

        public SqlQueryBuilder WhereIn<T>(string column, IEnumerable<T> values)
        {
            if (values == null || !values.Any())
            {
                return this;
            }

            var paramName = _parameters.GetNextParameterName();

            _whereConditions.Add(string.Concat(column, " IN ", paramName));

            _parameters.Add(paramName, values);

            return this;
        }

        public SqlQueryBuilder WhereNotIn<T>(string column, IEnumerable<T> values)
        {
            if (values == null || !values.Any())
            {
                return this;
            }

            var paramName = _parameters.GetNextParameterName();

            _whereConditions.Add(string.Concat(column, " NOT IN ", paramName));

            _parameters.Add(paramName, values);

            return this;
        }

        public DataQuery<T> BuildQuery<T>()
        {
            var selectQuery = BuildSelectQuery();
            var query = new DataQuery<T>(_connection, selectQuery, _parameters);
            Reset();
            return query;
        }
        public DataQuery<long> BuildSelectCountQuery()
        {
            var selectQuery = BuildCountQuery();
            var query = new DataQuery<long>(_connection, selectQuery, _parameters);

            Reset();
            return query;
        }
        public PagedDataQuery<T> BuildPagedQuery<T>(SearchCriteria searchCriteria)
        {
            Ensure.That(searchCriteria, nameof(searchCriteria)).IsNotNull();

            if (_sortingBy == null)
            {
                SetSortingBy(searchCriteria.IsAscending);
            }

            var selectQuery = BuildSelectQuery(searchCriteria.PageNumber, searchCriteria.PageSize);
            var countQuery = BuildCountQuery();
            var query = new PagedDataQuery<T>(_connection, selectQuery, countQuery, _parameters, searchCriteria);

            Reset();
            return query;
        }
        private string BuildSelectQuery()
        {
            var builder = new StringBuilder();
            builder.Append("SELECT ");

            if (_isDistinct)
            {
                builder.Append("DISTINCT ");
            }

            builder.Append(string.Join(", ", _columnsToSelect.Where(c => !string.IsNullOrEmpty(c))));

            builder.Append($" FROM {_dataSource} ");

            if (_whereConditions.Any())
            {
                builder.Append(" WHERE " + string.Join(" AND ", _whereConditions.Where(c => !string.IsNullOrEmpty(c))));
            }

            return builder.ToString();
        }
        private void SetSortingBy(bool isAscending)
        {
            _sortingBy = new SortColumn(_columnsToSelect.First(x => !x.Contains(" AS ")), isAscending);
        }
        private string BuildSelectQuery(int? pageNumber = null, int? pageSize = null)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT ");

            if (_isDistinct)
            {
                builder.Append("DISTINCT ");
            }

            if (_topCount.HasValue)
            {
                builder.Append($"TOP {_topCount} ");
            }

            if (_sum)
            {
                builder.Append(string.Join(", ", _columnsToSelect.Where(c => !string.IsNullOrEmpty(c)).Select(x => "ISNULL(SUM(" + x + "), 0) AS " + x)));
            }
            else
            {
                builder.Append(string.Join(", ", _columnsToSelect.Where(c => !string.IsNullOrEmpty(c))));
            }

            builder.Append($" FROM {_dataSource} ");

            if (_whereConditions.Any())
            {
                builder.Append(" WHERE " + string.Join(" AND ", _whereConditions.Where(c => !string.IsNullOrEmpty(c))));
            }

            if (_sortingBy != null)
            {
                builder.Append($" ORDER BY {_sortingBy.Column} {_sortingBy.Direction}");
            }
            else if (_isRandom)
            {
                builder.Append(" ORDER BY NEWID()");
            }

            if (pageNumber != null && pageSize != null)
            {
                Ensure.That(pageSize.Value, nameof(pageSize)).IsGt(0);
                Ensure.That(pageNumber.Value, nameof(pageNumber)).IsGt(0);
                builder.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageNumber - 1) * pageSize, pageSize);
            }

            return builder.ToString();
        }

        private string BuildCountQuery()
        {
            var builder = new StringBuilder();

            if (!_isDistinct)
            {
                builder.Append("SELECT COUNT(*) FROM ");
            }
            else
            {
                builder.Append("SELECT COUNT(DISTINCT " + string.Join(",", _columnsToSelect) + " ) FROM ");
            }

            builder.Append(_dataSource);

            if (_whereConditions.Any())
            {
                builder.Append($" WHERE {string.Join(" AND ", _whereConditions)}");
            }

            return builder.ToString();
        }

        private void AddFilter(string column, string filterOperator, object valueToFilter)
        {
            Ensure.String.IsNotNullOrWhiteSpace(column, nameof(column));
            Ensure.String.IsNotNullOrWhiteSpace(filterOperator, nameof(filterOperator));
            EnsureArg.IsNotNull(valueToFilter, nameof(valueToFilter));

            var paramName = _parameters.GetNextParameterName();
            _parameters.Add(paramName, valueToFilter);

            _whereConditions.Add(string.Concat(column, filterOperator, paramName));
        }

        private void Reset()
        {
            _columnsToSelect = new List<string>();
            _dataSource = null;
            _whereConditions = new List<string>();

            _parameters = new SqlQueryParameters();
            _isDistinct = false;
        }
    }
}
