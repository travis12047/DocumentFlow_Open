using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DocumentFlow.Models.ViewModels
{
	/// <summary>
	/// ログイン画面のViewModel
	/// </summary>
	public class AccountViewModel
	{
		/// <summary>
		/// 画面で入力されたログインID
		/// </summary>
		[Required(ErrorMessage = "{0}は必須です")]
		[StringLength(20)]
		[DisplayName("ログインID")]
		public string? name { get; set; }

        /// <summary>
        /// 画面で入力されたパスワード
        /// </summary>
        [Required(ErrorMessage = "{0}は必須です")]
        [StringLength(20)]
        [DisplayName("パスワード")]
        public string? pass { get; set; }
	}
}
