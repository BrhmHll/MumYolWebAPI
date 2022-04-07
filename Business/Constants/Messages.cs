using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
	public static class Messages
	{
		public static string Success = "Success";
		public static string Error = "Error";

		public static string AuthorizationDenied = "Yetkilendirme reddedildi!";

		public static string UserRegistered = "Kayıt başarılı!";
		public static string UserNotFound = "Kullanıcı bulunamadı!";
		public static string PasswordError = "Hatalı şifre!";
		public static string SuccessfulLogin = "Giriş başarılı.";
		public static string UserAlreadyExists = "Kullanıcı zaten mevcut";
		public static string AccessTokenCreated = "Token oluşturuldu.";

		public static string MaintenanceTime = "Sistem bakımda";
		public static string ProductListed = "Ürünler listelendi";
		public static string ProductAdded = "Ürün eklendi";
		public static string ProductNameAlreadyExists = "Ürün ismi zaten mevcut";
		public static string CategoryLimitExceded = "Kategori Limiti dolu";

		public static string TooManyProductInThisCategory = "Bu kategorideki ürün sayısı maksimumda.";
	}
}
