var gulp	= require("gulp");
var merge	= require('merge-stream');

var config = {
	dir: {
		node_modules: "../../node_modules",
		wwwroot: "./wwwroot"
	}
};

gulp.task("default", ["vendor"]);

gulp.task("vendor", [
	"vendor.scripts",
	"vendor.styles"
]);
gulp.task("vendor.scripts", [
	"vendor.scripts.jquery",
	"vendor.scripts.bootstrap"
]);
gulp.task("vendor.styles", [
	"vendor.styles.bootstrap"
]);

// jQuery
gulp.task("vendor.scripts.jquery", function() {
	var scripts =
		gulp.src(config.dir.node_modules + "/jquery/dist/jquery*.js")
			.pipe(
				gulp.dest(config.dir.wwwroot + "/scripts")
			);

	return scripts;
});

// Bootstrap
gulp.task("vendor.scripts.bootstrap", function() {
	var scripts =
		gulp.src(config.dir.node_modules + "/bootstrap/dist/js/bootstrap*.js")
			.pipe(
				gulp.dest(config.dir.wwwroot + "/scripts")
			);

	return scripts;
});
gulp.task("vendor.styles.bootstrap", function() {
	var css =
		gulp.src(config.dir.node_modules + "/bootstrap/dist/css/bootstrap*.css")
			.pipe(
				gulp.dest(config.dir.wwwroot + "/styles")
			);

	var fonts =
		gulp.src(config.dir.node_modules + "/bootstrap/dist/fonts/*.*")
			.pipe(
				gulp.dest(config.dir.wwwroot + "/fonts")
			);

	return merge(css, fonts);
});
