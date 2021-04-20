using IdentityPosgre.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPosgre.Servise
{
    public interface IRepository
    {
        Task<UserManagerResponce> CreateAsync(Person person);
        Task<UserDataResponse<Person>> GetSimpleUser();
        Task<UserDataResponse<Person>> GetAllData();
    }
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _con;

        public Repository(ApplicationDbContext context)
        {
            _con = context;
        }
        public async Task<UserManagerResponce> CreateAsync(Person person)
        {
            if (person == null)
                return new UserManagerResponce() { IsSuccsess = false, Message = "model can not be null" };
            await _con.People.AddAsync(person);
            await _con.SaveChangesAsync();
            return new UserManagerResponce() { IsSuccsess = true, Message = "model add succsessfully" };
        }

        public async Task<UserDataResponse<Person>> GetAllData()
        {
            var data = await _con.People.ToListAsync();
            return new UserDataResponse<Person>() { IsSuccsess = true, Message = "successfull", Data = data };
        }

        public async Task<UserDataResponse<Person>> GetSimpleUser()
        {
            var data = await _con.People.Select(i => new Person
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                Malumoti = i.Malumoti,
                Region = i.Region
            }).ToListAsync();
            return new UserDataResponse<Person>() { IsSuccsess = true, Message = "successfull", Data = data };
        }
    }
}
