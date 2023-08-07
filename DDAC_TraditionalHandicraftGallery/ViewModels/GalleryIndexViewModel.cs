using DDAC_TraditionalHandicraftGallery.Models;
using System.Collections.Generic;

namespace DDAC_TraditionalHandicraftGallery.ViewModels
{
	public class GalleryIndexViewModel
	{
		public List<Handicraft> Handicrafts { get; set; }
		public List<HandicraftType> HandicraftTypes { get; set; }
	}
}
