using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.BLL.Account
{
    public class AccountUpdates : IAccountUpdates
    {
        // Dependency Injections
        private readonly IAccountRepository _repo;
        public AccountUpdates(IAccountRepository repo)
        {
            _repo = repo;

        }
        public async Task<ActionResult<AppUser>> ProcessUpdateProfile(UpdateProfileFormDto updateProfileForm)
        {
            var (username, dateOfBirth, country, gender, bio) = updateProfileForm;

            bool allFieldsEmpty = AreAllFieldsEmpty(updateProfileForm);
            if (allFieldsEmpty)
                return new StatusCodeResult(304);

            var fieldsToUpdate = FieldsToUpdate(updateProfileForm);
            return await _repo.UpdateProfile(updateProfileForm, fieldsToUpdate);


        }


        // Private Methods:

        /// <summary>
        /// Process the update form to check if all required fields are empty.
        /// </summary>
        /// <param name="updateProfileForm">The client submitted update form.</param>
        /// <returns>
        /// <paramref name="True"/> - If all fields are empty. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - If there's at least one field that's not empty.
        /// </returns>
        private bool AreAllFieldsEmpty(UpdateProfileFormDto updateProfileForm)
        {
            foreach (PropertyInfo field in updateProfileForm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.Name == "Username")
                    continue;
                if (!string.IsNullOrEmpty((string?)field.GetValue(updateProfileForm)))
                    return false;
            }
            return true;
        }


        /// <summary>
        /// This method determines which are the fields that need to be updated.
        /// </summary>
        /// <param name="updateProfileForm">The client submitted update form.</param>
        /// <returns>A list with the names of the fields that need to be updated</returns>
        private List<PropertyInfo> FieldsToUpdate(UpdateProfileFormDto updateProfileForm)
        {
            List<PropertyInfo> fieldsToUpdate = new List<PropertyInfo>();
            foreach (PropertyInfo field in updateProfileForm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.Name == "Username")
                    continue;
                if (string.IsNullOrEmpty((string?)field.GetValue(updateProfileForm)))
                    continue;
                fieldsToUpdate.Add(field);
            }
            return fieldsToUpdate;
        }
    }
}