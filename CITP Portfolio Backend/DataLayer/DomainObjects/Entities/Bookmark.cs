﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class Bookmark
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }


    public UserBookmark BookmarkedBy { get; set; }

}