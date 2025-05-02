using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Application.Services.Interfaces;
using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;
using DiscplinaMobileNoite.Shared.Logging;
using DiscplinaMobileNoite.Shared.Validator;
using Serilog;

namespace DiscplinaMobileNoite.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public UserService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<UserEntity>> Add(UserEntity userEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidUser = await IsValidUserRequest(userEntity);

                if (!isValidUser.Success)
                {
                    Log.Error(LogMessages.InvalidUserInputs());
                    return Result<UserEntity>.Error(isValidUser.Message);
                }

                var checkEmail = await _repositoryUoW.UserRepository.GetByEmail(userEntity.Email);

                if (checkEmail is not null)
                {
                    Log.Error(LogMessages.InvalidEmail());
                    return Result<UserEntity>.Error("This email already exists. Please use another one.");
                }

                userEntity.Email = userEntity.Email?.Trim().ToLower();
                var result = await _repositoryUoW.UserRepository.Add(userEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<UserEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new User");
            }
            finally
            {
                Log.Error(LogMessages.AddingUserSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<UserEntity>> Update(UserResponse userResponse)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var userById = await _repositoryUoW.UserRepository.GetById(userResponse.Id);
                if (userById is null)
                    throw new InvalidOperationException("Error updating User");

                if (userResponse.Password != userResponse.ConfirmPassword)
                    throw new InvalidOperationException("the passwords are different");

                userById.FullName = userResponse.FullName;
                userById.Email = userResponse.Email;
                userById.Password = userResponse.Password;               
                
                _repositoryUoW.UserRepository.Update(userById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<UserEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorUser(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating User", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<UserEntity>> IsValidUserRequest(UserEntity userEntity)
        {
            var requestValidator = await new UserRequestValidator().ValidateAsync(userEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<UserEntity>.Error(errorMessage);
            }

            return Result<UserEntity>.Ok();
        }
    }
}