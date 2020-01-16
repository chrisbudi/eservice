using E.ServiceCore.Identity.Api.Models;
using E.ServiceCore.Identity.Api.Service.Interfaces;
using E.ServiceCore.Identity.Data.Enities;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;

namespace E.ServiceCore.Identity.Api.Service
{
    public class LdapAuthenticationService : IAuthService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";


        private readonly LdapConfig _config;
        private readonly LdapConnection _connection;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            _config = config.Value;
            _connection = new LdapConnection
            {
                SecureSocketLayer = false
            };
        }

        public ApplicationUser Login(string username, string password)
        {
            _connection.Connect("10.255.141.134", 389);
            _connection.Bind("gagas\\e-procurement", _config.BindCredentials);


            var searchFilter = string.Format(_config.SearchFilter, username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
                false
            );

            try
            {
                var user = result.next();

                //var userCount = result;
                //var listUser = new List<string>();
                //if (result != null)
                //{


                //    LdapEntry nextEntry = null;
                //    try
                //    {
                //        while (result.hasMore())
                //        {
                //            nextEntry = result.next();

                //            listUser.Add(nextEntry.getAttribute(SAMAccountNameAttribute).StringValue);
                //        }


                //    }
                //    catch (LdapException e)
                //    {

                //        throw new Exception("Login failed.");

                //    }


                //}

                if (user != null)
                {
                    _connection.Bind(user.DN, password);

                    if (_connection.Bound)
                    {
                        return new ApplicationUser
                        {
                            FirstName = user.getAttribute(DisplayNameAttribute).StringValue,
                            UserName = user.getAttribute(SAMAccountNameAttribute).StringValue,
                            //IsAdmin = user.getAttribute(MemberOfAttribute).StringValueArray.Contains(_config.AdminCn)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();
            return null;
        }

        public ApplicationUser LoginSSO(string username)
        {
            _connection.Connect("10.255.141.134", 389);
            _connection.Bind("gagas\\e-procurement", _config.BindCredentials);


            var searchFilter = string.Format(_config.SearchFilter, username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
                false
            );

            try
            {
                var user = result.next();

                var userCount = result;
                var listUser = new List<string>();
                if (result != null)
                {


                    LdapEntry nextEntry = null;
                    try
                    {
                        var anyData = false;
                        while (result.hasMore())
                        {
                            nextEntry = result.next();

                            listUser.Add(nextEntry.getAttribute(SAMAccountNameAttribute).StringValue);

                            if (username == nextEntry.getAttribute(SAMAccountNameAttribute).StringValue)
                            {
                                anyData = true;
                                break;
                            }
                            else
                            {
                                anyData = false;
                            }
                        }


                        if (anyData)
                        {
                            return new ApplicationUser()
                            {
                                FirstName = user.getAttribute(DisplayNameAttribute).StringValue,
                                UserName = user.getAttribute(SAMAccountNameAttribute).StringValue,
                                //            //IsAdmin = user.getAttribute(MemberOfAttribute).StringValueArray.Contains(_config.AdminCn)
                            };
                        }
                        else { return null; }

                    }
                    catch (LdapException e)
                    {

                        throw new Exception("Login failed.");

                    }


                }

                //if (user != null)
                //{
                //    _connection.Bind(user.DN, password);

                //    if (_connection.Bound)
                //    {
                //        return new ApplicationUser
                //        {
                //            FirstName = user.getAttribute(DisplayNameAttribute).StringValue,
                //            UserName = user.getAttribute(SAMAccountNameAttribute).StringValue,
                //            //IsAdmin = user.getAttribute(MemberOfAttribute).StringValueArray.Contains(_config.AdminCn)
                //        };
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();
            return null;
        }


    }
}
