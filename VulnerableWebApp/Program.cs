using Microsoft.EntityFrameworkCore;
using VulnerableWebApp;

var builder = WebApplication.CreateBuilder(args);

// 1) Ajouter le DbContext InMemory
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("VulnerableDb"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=VulnerableDb;Trusted_Connection=True;TrustServerCertificate=True;")

);

// 2) Ajouter les contrôleurs MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Activer le routing, etc.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// 3) Configurer la route par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ajout d'un utilisateur "admin" (pw = 12345)
    db.UserAccounts.Add(new UserAccount 
    { 
        Username = "admin", 
        Password = "12345"
    });

    // Ajout d'un utilisateur "bob" (pw = bobpass)
    db.UserAccounts.Add(new UserAccount 
    { 
        Username = "bob", 
        Password = "bobpass"
    });

    db.SaveChanges();
}


app.Run();

// ----------------------------------
// Notre DbContext minimal
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public required DbSet<UserAccount> UserAccounts { get; set; }
    public required DbSet<Comment> Comments { get; set; }
}

// Exemple de modèle
public class UserAccount
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

// Exemple de modèle
namespace VulnerableWebApp
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }
    }
}
