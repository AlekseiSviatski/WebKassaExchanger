namespace WebKassa.Models.DBModel
{
	/// <summary>
	/// Класс товара (услуги)
	/// </summary>
	public class SingleServiceModel
	{
		/// <summary>
		/// ID товара (услуги)
		/// </summary>
		public int? ID { get; set; }

		/// <summary>
		/// ID группы товара (услуги)
		/// </summary>
		public int? SingleServiceGroupID { get; set; }

		/// <summary>
		/// Порядок сортировки
		/// </summary>
		public int? SortOrder { get; set; }

		/// <summary>
		/// Название товара (услуги)
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// Цена за единицу
		/// </summary>
		public decimal? Price { get; set; }

		/// <summary>
		/// Цена за единицу при поступлении (в парию)
		/// </summary>
		public decimal? PriceReceipt { get; set; }

		/// <summary>
		/// Цена реализации/продважи (в партии)
		/// </summary>
		public decimal? PriceDistribution { get; set; }

		/// <summary>
		/// Артикул/код из 1С
		/// </summary>
		public string? Code { get; set; }

		/// <summary>
		/// Штрихкод
		/// </summary>
		public string? Barcode { get; set; }

		/// <summary>
		/// Общее кол-во товара
		/// </summary>
		public decimal? CountTotal { get; set; }

		/// <summary>
		/// Общий остаток товара
		/// </summary>
		public decimal? CountRemaining { get; set; }

		/// <summary>
		/// Единица измерения
		/// </summary>
		public string? Unit { get; set; }
	}
}