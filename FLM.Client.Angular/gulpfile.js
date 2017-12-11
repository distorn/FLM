/// <binding />
var gulp = require('gulp');
var gutil = require('gulp-util');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var minifyCss = require('gulp-minify-css');
var rename = require('gulp-rename');
var less = require('gulp-less');
var babel = require('gulp-babel');
var ngAnnotate = require('gulp-ng-annotate');
var ngConfig = require('gulp-ng-config');
var del = require('del');

// - src folders -

var project_root = "";
var public_root = project_root + "wwwroot/";
var app_root = public_root + "app/";
var lib_root = public_root + "lib/";
var less_root = public_root + "less/";
var css_root = public_root + "css/";

// - publish folders -

var compiled_content = public_root + "content/";

// - bundles -

var all_libs = [
	lib_root + "angular/angular.js",
	lib_root + "ui-router/release/angular-ui-router.js",
	lib_root + "jquery/dist/jquery.min.js",
	lib_root + "bootstrap/dist/js/bootstrap.min.js",
	lib_root + "angular-messages/angular-messages.js",
	lib_root + "angular-animate/angular-animate.js",
	lib_root + "angular-sanitize/angular-sanitize.js",
	lib_root + "angular-bootstrap/ui-bootstrap-tpls.js",
	lib_root + "angular-local-storage/dist/angular-local-storage.js",
	lib_root + "angular-ui-select/dist/select.js",
	lib_root + "ng-lodash/build/ng-lodash.min.js"
];

var all_scripts = [
	app_root + "**/*module.js",
	app_root + "**/*.js"
];

var all_less = [
	less_root + "*.less"
];

var all_css = [
	css_root + "*.css",
	lib_root + "angular-ui-select/dist/select.min.css"
];

// - tasks -

// todo: add watchers https://medium.com/@dickeyxxx/best-practices-for-building-angular-js-apps-266c1a4a6917

gulp.task('build:config-dev', function () {
	gulp.src(project_root + "app.config.json")
		.pipe(ngConfig('FLMClientApp.config', { environment: "env.dev" }))
		.pipe(gulp.dest(compiled_content));
});

gulp.task('build:config-production', function () {
	gulp.src(project_root + "app.config.json")
		.pipe(ngConfig('FLMClientApp.config', { environment: "env.production" }))
		.pipe(gulp.dest(compiled_content));
});

gulp.task('build:js-libs', function () {
	gulp.src(all_libs)
		.pipe(concat("libs.min.js"))
		.pipe(uglify())
		.pipe(gulp.dest(compiled_content));
});

gulp.task('build:js-app', function () {
	gulp.src(all_scripts)
		.pipe(concat("app.min.js"))
		.pipe(babel({
			presets: ['es2015']
		}))
		.pipe(ngAnnotate())
		.pipe(uglify().on('error', function (err) {
			gutil.log(gutil.colors.red('[Error]'), err.toString());
			this.emit('end');
		}))
		.pipe(gulp.dest(compiled_content));
});

gulp.task('build:less', function () {
	return gulp.src(all_less)
		.pipe(less())
		.pipe(concat("less-compiled.css"))
		.pipe(gulp.dest(css_root));
});

gulp.task('build:css', ['build:less'], function () {
	gulp.src(all_css)
		.pipe(concat("all.min.css"))
		.pipe(minifyCss())
		.pipe(gulp.dest(compiled_content));
});

gulp.task('build:style', [
	'build:less',
	'build:css',
], function () { });

gulp.task('full-build:dev', [
	'build:style',
	'build:js-libs',
	'build:js-app',
	'build:config-dev'
], function () { });

gulp.task('full-build:release', [
	'build:style',
	'build:js-libs',
	'build:js-app',
	'build:config-production'
], function () { });

gulp.task("default", ["full-build:dev"], function () { });