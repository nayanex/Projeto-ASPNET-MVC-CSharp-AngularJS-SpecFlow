module.exports = function(grunt) {
  'use strict';
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    connect: {
      options: {
        port: 9000,
        hostname: '*',
        livereload: 35729
      },
      livereload: {
        options: {
          open: true,
          base: ['../', './', '.links']
        }
      }
    },
    watch: {
      livereload: {
        options: {
          livereload: '<%= connect.options.livereload %>'
        },
        files: [
          '../Scripts/{,*/}*.js',
          './**/*.js',
          './{,*/}*.html'
        ]
      }
    },
  });

  //loading tasks
  grunt.loadNpmTasks('grunt-contrib-connect');
  grunt.loadNpmTasks('grunt-contrib-watch');
  //register tasks
  grunt.registerTask('server', [
      'connect:livereload',
      'watch'
  ]);
}
