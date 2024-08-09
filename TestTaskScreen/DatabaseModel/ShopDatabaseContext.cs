using Microsoft.EntityFrameworkCore;

namespace TestTaskScreen.DatabaseModel
{
	internal class ShopDatabaseContext : DbContext
	{
		/// <summary>
		/// Пользователи магазина
		/// </summary>
		public DbSet<User> Users { get; protected set; }

		/// <summary>
		/// Товары магазина
		/// </summary>
		public DbSet<Product> Products { get; protected set; }

		/// <summary>
		/// Срезы состояний товаров магазина
		/// </summary>
		public DbSet<ProductSlice> ProductSlices { get; protected set; }

		/// <summary>
		/// Покупки
		/// </summary>
		public DbSet<Purchase> Purchases { get; protected set; }

		public ShopDatabaseContext(DbContextOptions options) : base(options) {}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(user =>
			{
				user.HasKey(x => x.Id);
				user.HasAlternateKey(x => x.Email);
			});
			modelBuilder.Entity<Image>().HasKey(x => x.Id);

			modelBuilder.Entity<Product>(product =>
			{
				product.HasKey(x => x.Id);
				product.HasMany(x => x.Images).WithMany();
				
			});

			modelBuilder.Entity<ProductSlice>(slice =>
			{
				slice.HasKey(x => x.Id);
				slice.HasMany(x => x.Images).WithMany();
				slice.HasOne(x => x.OriginalProduct).WithMany();
			});

			modelBuilder.Entity<Purchase>(purchase =>
			{
				purchase.HasKey(x => x.Id);
				purchase.HasOne(x => x.User).WithMany();
				purchase.HasMany(x => x.Products).WithOne(x => x.Purchase);
			});

			modelBuilder.Entity<PurchaseProduct>(pp =>
			{
				pp.HasKey(x => new { x.PurchaseId, x.ProductId });
				pp.HasOne(x => x.Product).WithMany();
				pp.HasOne(x => x.Purchase).WithMany(x => x.Products);
			});
		}
	}
}
