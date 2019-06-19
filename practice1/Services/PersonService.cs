using practice1.Helpers;
using practice1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practice1.Services
{
    public interface IPersonService
    {
        Person Authenticate(string login, string password);
        IEnumerable<Person> GetAll();
        Person GetById(int id);
        Person Create(Person person, string password);
        void Update(Person person, string password = null);
        void Delete(int id);
    }

    public class PersonService : IPersonService
    {
        private DataContext _context;

        public PersonService(DataContext context)
        {
            _context = context;
        }

        public Person Authenticate(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var person = _context.People.SingleOrDefault(x => x.Login == login);

            // check if username exists
            if (person == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, person.PasswordHash, person.PasswordSalt))
                return null;

            // authentication successful
            return person;
        }

        public IEnumerable<Person> GetAll()
        {
            return _context.People;
        }

        public Person GetById(int id)
        {
            return _context.People.Find(id);
        }

        public Person Create(Person person, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.People.Any(x => x.Login == person.Login))
                throw new AppException("Login \"" + person.Login + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;

            _context.People.Add(person);
            _context.SaveChanges();

            return person;
        }

        public void Update(Person personParam, string password = null)
        {
            var person = _context.People.Find(personParam.Id);

            if (person == null)
                throw new AppException("User not found");

            if (personParam.Login != person.Login)
            {
                // username has changed so check if the new username is already taken
                if (_context.People.Any(x => x.Login == personParam.Login))
                    throw new AppException("Login " + personParam.Login + " is already taken");
            }

            // update user properties
            person.Login = personParam.Login;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                person.PasswordHash = passwordHash;
                person.PasswordSalt = passwordSalt;
            }

            _context.People.Update(person);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var person = _context.People.Find(id);
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
