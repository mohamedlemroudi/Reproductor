package com.example.appmusicandroid.Adaper

import com.example.appmusicandroid.Model.MusicModel

interface IOnItemClickListener {
    fun onItemClick(item: MusicModel, position: Int)
}