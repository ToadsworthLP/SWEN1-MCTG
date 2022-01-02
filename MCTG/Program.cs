using MCTG.Auth;
using MCTG.Config;
using MCTG.Controllers;
using MCTG.Gameplay;
using MCTG.Services;
using Rest;

namespace MCTG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RestServer server = new RestServer(System.Net.IPAddress.Any, 10001);

            server.RequestFinished += (sender, args) => AppDbContext.NotifyEndOfRequest();

            server.AddController<UserController>();
            server.AddController<SessionController>();
            server.AddController<StatsController>();
            server.AddController<ScoreboardController>();
            server.AddController<PackageController>();
            server.AddController<PackageTransactionController>();
            server.AddController<CardsController>();
            server.AddController<DeckController>();
            server.AddController<TradeController>();

            server.AddScoped<IPasswordHashService, PasswordHashService>();
            server.AddScoped<ITokenService, TokenService>();
            server.AddScoped<ICardNameService, CardNameService>();
            server.AddScoped<ICardUsageCheckService, CardUsageCheckService>();

            server.AddScoped<AppDbContext>();

            CardTypeRegistry cardTypeRegistry = new CardTypeRegistry();
            cardTypeRegistry.AddDefaultCardTypes();
            server.AddSingletonInstance(cardTypeRegistry);

            server.AddSingletonInstance<ICardElementDamageCalculator>(new DefaultCardElementDamageCalculator());

            server.AddAuth<AuthProvider>();

            server.Start();
        }
    }
}