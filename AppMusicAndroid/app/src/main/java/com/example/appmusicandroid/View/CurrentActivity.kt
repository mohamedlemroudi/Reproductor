package com.example.appmusicandroid.View

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.view.View
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.Toast
import androidx.appcompat.widget.SearchView
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.appmusicandroid.Adaper.CurrentAdapter
import com.example.appmusicandroid.Adaper.IOnItemClickListener
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.Music.FindMusic
import com.example.appmusicandroid.Music.IFindMusic
import com.example.appmusicandroid.Music.MediaPlayerController
import com.example.appmusicandroid.R
import com.example.appmusicandroid.ViewModel.CurrentViewModel
import com.example.appmusicandroid.databinding.ActivityCurrentBinding
import java.util.ArrayList
import java.util.Locale

class CurrentActivity : AppCompatActivity() {

    private lateinit var binding: ActivityCurrentBinding

    private lateinit var findMusic: IFindMusic

    private val musicList = ArrayList<MusicModel>()

    private val viewModel by lazy {
        ViewModelProvider(this, defaultViewModelProviderFactory).get(CurrentViewModel::class.java)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityCurrentBinding.inflate(layoutInflater)
        setContentView(binding.root)
        initUI()
        initListener()
    }

    private fun initUI(){
        findMusic = FindMusic()

        viewModel.checkPermission(findMusic, applicationContext)

        viewModel.getMusic(this)
        getMusic()
    }

    private fun initListener() {
        binding.Playlist.setOnClickListener {
            val intent = Intent(this@CurrentActivity, CloudMusicActivity::class.java)
            startActivity(intent)
        }

        binding.CrearCanco.setOnClickListener {
            val intent = Intent(this@CurrentActivity, UploadSongActivity::class.java)
            startActivity(intent)
        }
    }

    override fun onResume() {
        super.onResume()
        search()
        btnClick()
    }

    private fun getMusic(){
        viewModel.musicList.observe(this) {
            if (it.size > 0) {
                println("liste " + it.size)
                musicList.addAll(it)
                initRecycler(it)
            }
        }
    }

    private fun search(){
        binding.search.setOnQueryTextListener(object : SearchView.OnQueryTextListener,
            android.widget.SearchView.OnQueryTextListener {
            override fun onQueryTextSubmit(query: String?): Boolean {
                return true
            }
            override fun onQueryTextChange(newText: String?): Boolean {
                filter1(newText!!)
                return true
            }
        })
    }

    private fun filter1(text : String){
        val filterList = ArrayList<MusicModel>()

        musicList.forEach {
            if (it.title.toLowerCase(Locale.ROOT).contains(text.toLowerCase(Locale.ROOT)) ||
                it.artist.toLowerCase(Locale.ROOT).contains(text.toLowerCase(Locale.ROOT))   ){
                filterList.add(it)
            }
        }

        if (filterList.isEmpty()){
            Toast.makeText(this, "Music is not found", Toast.LENGTH_SHORT).show()
        }else{
            initRecycler(filterList)
        }
    }
    private fun initRecycler(musicModelList : ArrayList<MusicModel>){
        val adapter = CurrentAdapter(
            musicModelList,
            object: IOnItemClickListener {
                override fun onItemClick(item: MusicModel, position: Int) {
                    closeCurrentMusic()
                    openMusicPlayerActivity(musicList.indexOf(item))
                }
            })
        binding.recyclerView.adapter = adapter
        binding.recyclerView.layoutManager = LinearLayoutManager(this)
        adapter.setFilterList(musicModelList)
    }

    private fun btnClick(){

        binding.btnStop.setOnClickListener {
            closeCurrentMusic()
        }

        binding.btnShuffle.setOnClickListener {
            val random = (0..<musicList.size).random()
            closeCurrentMusic()
            openMusicPlayerActivity(random)
        }
    }
    private fun openMusicPlayerActivity(position: Int){
        val intent = Intent(this@CurrentActivity, MusicPlayerActivity::class.java)
        intent.putExtra("position", position)
        startActivity(intent)
        overridePendingTransition(R.anim.rigthtoleft1, R.anim.rigthtoleft2)
        finish()
    }

    private fun closeCurrentMusic(){
        MediaPlayerController.mediaPlayer?.stop()
    }

}