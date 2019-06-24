using practice1.Helpers;
using practice1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace practice1.Services
{
    public interface IUserService
    {
        //User Authenticate(string login, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user);
        void Update(User newuser);
        void Delete(int id);
    }

    public class PersonService : IUserService
    {
        private DataContext _context;

        public PersonService(DataContext context)
        {
            _context = context;
        }

        //public User Authenticate(string login, string password)
        //{
        //    throw new NotImplementedException();
        //}

        public User Create(User user)
        {
            //throw new NotImplementedException();

            _context.Users.Add(user);

            var salt = Salt.Create();
            var hash = Hash.Create(user.Password, salt);
            user.Password = hash;
               

            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            var usercomp = _context.Users
            .Include(c => c.Company)
            .ToList();
            return _context.Users;

        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Update(User newuser)
        {
            var user1  = _context.Users.Find(newuser.UserId);

            user1.Name = newuser.Name;
            user1.Address = newuser.Address;
            user1.PhoneNumber = newuser.PhoneNumber;
            
            var salt = Salt.Create();
            var hash = Hash.Create(user1.Password, salt);
            user1.Password = hash;
            _context.SaveChanges();
        }
    }
}
