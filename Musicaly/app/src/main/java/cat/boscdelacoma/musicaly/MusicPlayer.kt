package cat.boscdelacoma.musicaly

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.ImageView
import android.widget.PopupMenu

class MusicPlayer : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_music_player)
        val iconRight = findViewById<ImageView>(R.id.IconRight)

        iconRight.setOnClickListener { view ->
            showPopupMenu(view)
        }
    }

    private fun showPopupMenu(view: View) {
        val popupMenu = PopupMenu(this, view)
        val inflater = popupMenu.menuInflater
        inflater.inflate(R.menu.menu_reproductor, popupMenu.menu)

        popupMenu.setOnMenuItemClickListener { menuItem ->
            when (menuItem.itemId) {
                R.id.opcion1 -> {
                    val intent = Intent(this, InfoAlbum::class.java)
                    startActivity(intent)
                    true
                }
                R.id.opcion2 -> {
                    val intent = Intent(this, InfoArtista::class.java)
                    startActivity(intent)
                    true
                }
                R.id.opcion3 -> {
                    true
                }
                else -> false
            }
        }

        popupMenu.show()
    }
}
