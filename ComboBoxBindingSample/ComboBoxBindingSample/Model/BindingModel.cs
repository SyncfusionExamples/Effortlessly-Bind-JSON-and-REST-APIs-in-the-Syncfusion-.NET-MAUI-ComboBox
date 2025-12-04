using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxBindingSample.Model
{
	/// <summary>
	/// Represents a country item used for ComboBox binding.
	/// </summary>
	public class Country
	{
		/// <summary>
		/// Gets or sets the display name of the country.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the country code (e.g., ISO code).
		/// </summary>
		public string Code { get; set; }
	}

	/// <summary>
	/// Simple user data transfer object used in REST API sample.
	/// </summary>
	public class UserDto
	{
		/// <summary>
		/// Gets or sets the unique identifier of the user.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		public string Name { get; set; }
	}
}
