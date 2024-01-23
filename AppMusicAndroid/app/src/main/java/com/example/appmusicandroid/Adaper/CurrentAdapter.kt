package com.example.appmusicandroid.Adaper

import android.annotation.SuppressLint
import android.text.method.ScrollingMovementMethod
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.request.RequestOptions
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.R
import com.example.appmusicandroid.databinding.CurrentRecyclerRowBinding

class CurrentAdapter (
    private var musicList:ArrayList<MusicModel>,
    private val listener: IOnItemClickListener
) : RecyclerView.Adapter<CurrentAdapter.CurrenViewHolder>(){

    class CurrenViewHolder(layout: View) : RecyclerView.ViewHolder(layout){
        val binding = CurrentRecyclerRowBinding.bind(layout)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CurrenViewHolder {
        return CurrenViewHolder(
            LayoutInflater
                .from(parent.context)
                .inflate(R.layout.current_recycler_row, parent, false)
        )
    }

    override fun getItemCount() = musicList.size

    override fun onBindViewHolder(holder: CurrenViewHolder, position: Int) {

        holder.binding.textName.setText(musicList.get(position).title.trim())

        holder.binding.textName.movementMethod = ScrollingMovementMethod()

        Glide.with(holder.itemView.context)
            .load(musicList.get(position).uriImage)
            .apply(RequestOptions().placeholder(R.drawable.cover).centerCrop())
            .error(R.drawable.cover)
            .into(holder.binding.imageView)

        holder.binding.recyclerRow.setOnClickListener {
            listener.onItemClick(musicList.get(position), position)
        }

        holder.binding.textName.setOnClickListener {
            listener.onItemClick(musicList.get(position), position)
        }
    }

    @SuppressLint("NotifyDataSetChanged")
    fun setFilterList(filterList: ArrayList<MusicModel>){
        musicList = filterList
        notifyDataSetChanged()
    }
}