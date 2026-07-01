> [!WARNING]
> in development.
 
# Serein

Serein is a lightweight desktop music player built with **C#** and **WPF**. Inspired by the simplicity of classic Windows media players and the soft aesthetic of forgotten software from the early 2000s, Serein focuses on one thing: enjoying your local music library.

Instead of streaming services, recommendations, or subscriptions, Serein is designed for people who still keep folders full of MP3s and enjoy owning their music collection.

---

## Features

- Local MP3 playback
- Album artwork extraction
- Artist filtering
- Song metadata
  - Title
  - Artist
  - Album Cover
  - Duration
- Playback controls
  - Play / Pause
  - Next / Previous
  - Seek bar

---

## Preview

> Screenshots coming soon.

---

## Built With

- C#
- .NET
- WPF
- Layered Architecture (Models • Services • Views)
- `MediaPlayer`
- TagLib#
- FontAwesome Sharp

---

## Project Structure

```text
Serein/
│
├── Models/
│   ├── Song.cs
│
├── Services/
│   ├── PlaylistLoaderService.cs
│   ├── MetadataService.cs
│   ├── PlaybackService.cs
│   └── ...
│
├── Views/
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
│
├── Styles/
│
├── App.xaml
└── serein.slnx
```
---

##  Roadmap

- [x] Local music playback
- [x] Album artwork
- [x] Artist filtering
- [x] Progress slider
- [ ] Song search
- [ ] Playlist support
- [ ] Shuffle & Repeat
- [ ] Keyboard shortcuts
- [ ] Theme customization
- [ ] Volume control

---

## License

This project is licensed under the [MIT licence](LICENSE)
