﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.ViewModels;

namespace API.Controllers
{
    [Route("api/Role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly MainDatabase _context;

        public RoleController(MainDatabase context)
        {
            _context = context;
        }
        [HttpGet, Route("GetByServer/{serverId}")]
        public async Task<ActionResult<IEnumerable<Role>>> GetByServer(int serverId) {
            return await _context.Role.Where(r => r.ServerId == serverId).ToListAsync();
        }
        [HttpGet, Route("GetUserRoleInServer/{userId}/{serverId}")]
        public async Task<IActionResult> GetUserRoleInServer(int userId, int serverId) {
            Role role = (await _context.ServerUser
                .Where(su => su.UserId == userId && su.ServerId == serverId)
                .Include("Role")
                .FirstOrDefaultAsync())
                .Role;
            return Ok(role);
        }
        [HttpPost, Route("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleViewModel model) {
            if(model == null) {
                return BadRequest();
            }
            ServerUser serverUser = await _context.ServerUser.Where(su => su.ServerId == model.ServerId && su.UserId == model.UserId).FirstOrDefaultAsync();
            if(serverUser == null) {
                return NotFound(model);
            }
            serverUser.RoleId = model.NewRoleId;
            await _context.SaveChangesAsync();
            return Ok(serverUser);
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole()
        {
            return await _context.Role.ToListAsync();
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Role.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(int id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();

            return role;
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.RoleId == id);
        }
    }
}
