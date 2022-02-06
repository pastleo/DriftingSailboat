mergeInto(LibraryManager.library, {
  IsMobile: function() {
    var unityContainer = document.getElementById('unity-container');
    return unityContainer && unityContainer.classList.contains('unity-mobile');
  },
});
