package com.example.appmusicandroid.View

import android.annotation.SuppressLint
import android.content.Context
import android.content.Intent
import androidx.appcompat.widget.SearchView
import android.media.MediaPlayer
import android.net.Uri
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.SeekBar
import androidx.lifecycle.ViewModelProvider
import com.bumptech.glide.Glide
import com.bumptech.glide.request.RequestOptions
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.Music.FindMusic
import com.example.appmusicandroid.Music.MediaPlayerController
import com.example.appmusicandroid.Music.MediaPlayerController.Companion.mPause
import com.example.appmusicandroid.Music.MediaPlayerController.Companion.mStart
import com.example.appmusicandroid.R
import com.example.appmusicandroid.Util.TimetoMSS.Companion.timestampToMSS
import com.example.appmusicandroid.ViewModel.MusicPlayerViewModel
import com.example.appmusicandroid.databinding.ActivityMusicPlayerBinding
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch

class MusicPlayerActivity : AppCompatActivity() {

    private lateinit var binding : ActivityMusicPlayerBinding

    private var mediaPlayer : MediaPlayer? = null

    private lateinit var context: Context

    private lateinit var uri : Uri

    private var counterIcon = 0

    private var counterPositon = 0

    private val viewModel by lazy {
        ViewModelProvider(this, defaultViewModelProviderFactory).get(MusicPlayerViewModel::class.java)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMusicPlayerBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val findMusic = FindMusic()
        viewModel.getMusicFile(findMusic, this)
        counterPositon = getPosition()
        getMusic(getPosition())

        // RETROCEDER ATRAS
        val iconLeft: ImageView = binding.IconLeft

        // Configurando el clic del ImageView
        iconLeft.setOnClickListener { // Lógica para retroceder aquí (por ejemplo, cerrar la actividad)
            onBackPressed()
        }
    }

    override fun onResume() {
        super.onResume()

        CoroutineScope(Dispatchers.Main).launch {
            initSeekBar()
        }
        seekBarChange()
    }

    private fun getMusic(position: Int){
        viewModel.musicList.observe(this) {
            val music = it.get(position)
            context = this
            uri = music.uri
            btnClick(it.size)
            initText(music)
            initMediaPlayer()
            onCompleteMusic(it.size)
        }
    }

    private fun initMediaPlayer(){
        mediaPlayer = MediaPlayer.create(this, uri)
        MediaPlayerController.mediaPlayer = mediaPlayer
        mStart()
    }

    private fun initText(musicModel: MusicModel){

        binding.textMusicName.text = musicModel.title

        CoroutineScope(Dispatchers.Main).launch {
            setTextDuration()
        }

        CoroutineScope(Dispatchers.Main).launch {
            currentTextTime()
        }

        Glide.with(applicationContext)
            .load(musicModel.uriImage)
            .apply(RequestOptions().placeholder(R.drawable.posterimagen).centerCrop())
            .error(R.drawable.posterimagen)
            .into(binding.imageView2)
    }

    private suspend fun setTextDuration(){
        binding.textDuration.text = mediaPlayer!!.duration.let { timestampToMSS(it) }
    }

    @SuppressLint("SetTextI18n")
    private suspend fun currentTextTime() {
        while (true){
            delay(1000)
            binding.textStart.text = mediaPlayer?.currentPosition?.let { timestampToMSS(it) }
        }
    }

    private fun seekBarChange() {
        binding.seekBar.setOnSeekBarChangeListener(object : SeekBar.OnSeekBarChangeListener{
            override fun onProgressChanged(seekBar: SeekBar?, progress: Int, fromUser: Boolean) {
                if (fromUser) mediaPlayer?.seekTo(progress.times(1000))
            }
            override fun onStartTrackingTouch(seekBar: SeekBar?) {
            }
            override fun onStopTrackingTouch(seekBar: SeekBar?) {
            }
        })
    }

    private suspend fun initSeekBar() {
        while (true){
            delay(1000)
            binding.seekBar.max = mediaPlayer?.duration?.div(1000) ?: 0
            mediaPlayer?.currentPosition?.let { binding.seekBar.setProgress(it.div(1000)) }
        }
    }

    private fun btnClick(listSize : Int){
        binding.btnPlayOrPause.setOnClickListener {
            if (counterIcon % 2 == 0){
                mPause()
                binding.btnPlayOrPause.setImageResource(R.drawable.play)
                counterIcon++
            }else{
                mStart()
                binding.btnPlayOrPause.setImageResource(R.drawable.pause)
                counterIcon++
            }
        }

        binding.btnNext.setOnClickListener {
            if (counterPositon < (listSize - 1)){
                binding.btnPlayOrPause.setImageResource(R.drawable.pause)
                counterIcon++
                mediaPlayer?.stop()
                getMusic(++counterPositon)
            }
        }

        binding.btnPrev.setOnClickListener {
            if (counterPositon > 0){
                binding.btnPlayOrPause.setImageResource(R.drawable.pause)
                counterIcon++
                mediaPlayer?.stop()
                getMusic(--counterPositon)
            }
        }
    }

    private fun onCompleteMusic(listSize: Int){
        if (counterPositon < (listSize - 1)){
            mediaPlayer?.setOnCompletionListener {
                getMusic(++counterPositon)
            }
        }else{
            mediaPlayer?.isLooping = true
        }
    }

    private fun getPosition() : Int{
        return intent.getIntExtra("position", 0)
    }

    @Deprecated("Deprecated in Java")
    override fun onBackPressed() {
        super.onBackPressed()
        startActivity(Intent(this, CurrentActivity::class.java))
        overridePendingTransition(R.anim.lefttorigth1, R.anim.lefttorigth2)
        finish()
    }

}