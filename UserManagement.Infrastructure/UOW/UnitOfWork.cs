using UserManagement.Infrastructure.DataContext;
using UserManagement.Infrastructure.IRepositories;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Infrastructure.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly DapperContext _dapperContext;
    private readonly IDataLogin _dataLogin;

    public UnitOfWork(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    public IDataLogin Login
    {
        get
        {
            if (_dataLogin is null)
            {
                return new DataLogin(_dapperContext);
            }
            return _dataLogin;
        }
    }
}
