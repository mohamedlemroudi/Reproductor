package com.example.appmusicandroid.ViewModel

import android.view.View
import androidx.recyclerview.widget.RecyclerView
import com.example.appmusicandroid.Api.CloudMusicDataResponse
import com.example.appmusicandroid.Api.MusicItemResponse
import com.example.appmusicandroid.databinding.CurrentRecyclerRowBinding

class CloudMusicViewHolder(view: View): RecyclerView.ViewHolder(view) {

    private val binding = CurrentRecyclerRowBinding.bind(view)

    fun bind(songItemResponse: MusicItemResponse,
             onItemSelected:(String) -> Unit) {

        binding.textName.text = songItemResponse.title
        binding.root.setOnClickListener {
            onItemSelected(songItemResponse.uid)
        }
    }
}