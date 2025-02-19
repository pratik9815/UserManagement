﻿using UserManagement.Infrastructure.DataContext;
using UserManagement.Infrastructure.IRepositories;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Infrastructure.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly DapperContext _dapperContext;
    private readonly IDataLogin _dataLogin;
    private readonly IUserRepository _userRepository;
    private readonly IMenuRepository _menuRepository;
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
    public IUserRepository User
    {
        get
        {
            if( _userRepository is null)
                return new UserRepository(_dapperContext);
            return _userRepository;
        }
    }
    public IMenuRepository Menu
    {
        get
        {
            if(_menuRepository is null)
                return new MenuRepository(_dapperContext);
            return _menuRepository;
        }
    }
}
