package com.example.appmusicandroid.Adaper

import android.annotation.SuppressLint
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.appmusicandroid.Api.MusicItemResponse
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.R
import com.example.appmusicandroid.ViewModel.CloudMusicViewHolder

class CloudMusicAdapter(var musicList: List<MusicItemResponse> = emptyList(),
    private val onItemSelected:(String) -> Unit)
    : RecyclerView.Adapter<CloudMusicViewHolder>() {

    fun updateList(musicList: List<MusicItemResponse>){
        this.musicList = musicList
        notifyDataSetChanged()
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CloudMusicViewHolder {
        return CloudMusicViewHolder(
            LayoutInflater.from(parent.context).inflate(R.layout.current_recycler_row, parent, false)
        )
    }

    override fun onBindViewHolder(holder: CloudMusicViewHolder, position: Int) {
       holder.bind(musicList[position], onItemSelected)
    }

    override fun getItemCount() = musicList.size

    @SuppressLint("NotifyDataSetChanged")
    fun setFilterList(filterList: ArrayList<MusicItemResponse>){
        musicList = filterList
        notifyDataSetChanged()
    }
}