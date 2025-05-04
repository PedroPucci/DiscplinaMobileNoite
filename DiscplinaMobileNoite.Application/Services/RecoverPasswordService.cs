using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Application.Services.Interfaces;
using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;
using DiscplinaMobileNoite.Shared.Logging;
using Serilog;

namespace DiscplinaMobileNoite.Application.Services
{
    public class RecoverPasswordService : IRecoverPasswordService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public RecoverPasswordService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<RecoverPasswordResponse>> RecoverPasswordAsync(RecoverPasswordResponse recoverPasswordResponse)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var user = await _repositoryUoW.UserRepository.GetByEmail(recoverPasswordResponse.Email);

                if (user is null)
                {
                    Log.Error(LogMessages.InvalidEmail());
                    return Result<RecoverPasswordResponse>.Error("Email not found. Please check and try again.");
                }

                var newPassword = GenerateRandomPassword();

                user.Password = newPassword;

                _repositoryUoW.UserRepository.UpdateEmail(user.Email, user.Password);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                recoverPasswordResponse.Password = newPassword;
                return Result<RecoverPasswordResponse>.Ok(recoverPasswordResponse.Password);
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorRecoverPassword(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error found user email.");
            }
            finally
            {
                Log.Error(LogMessages.UpdatingSuccessRecoverPassword());
                transaction.Dispose();
            }
        }

        private string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}