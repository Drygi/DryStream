//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DryStream.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlaylistsSong
    {
        public int PlaylistsSongsID { get; set; }
        public int SongID { get; set; }
        public int PlaylistID { get; set; }
    
        public virtual Playlist Playlist { get; set; }
        public virtual Song Song { get; set; }
    }
}
